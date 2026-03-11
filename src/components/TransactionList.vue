<template>
  <div>
    <h2>Транзакции</h2>
    <div class="filters">
      <button @click="showAll">Все</button>
      <button @click="showDay = !showDay">За день</button>
      <button @click="showMonth = !showMonth">За месяц</button>
    </div>
    <div v-if="showDay">
      <input type="date" v-model="selectedDate" @change="fetchByDay" />
    </div>
    <div v-if="showMonth">
      <input type="number" v-model="selectedYear" min="2000" max="2100" placeholder="Год" @change="fetchByMonth" />
      <input type="number" v-model="selectedMonth" min="1" max="12" placeholder="Месяц" @change="fetchByMonth" />
    </div>
    <form @submit.prevent="addTransaction">
      <input v-model="newTransaction.date" type="date" required>
      <input v-model.number="newTransaction.amount" type="number" placeholder="Сумма" required>
      <input v-model="newTransaction.comment" placeholder="Комментарий">
      <select v-model.number="newTransaction.expenseItemId" required>
        <option disabled value="">Выберите статью</option>
        <option v-for="item in items" :key="item.id" :value="item.id" v-if="item.isActive">{{ item.name }}</option>
      </select>
      <button type="submit">Добавить</button>
    </form>
    <ul>
      <li v-for="t in transactions" :key="t.id">
        <strong>{{ t.date.slice(0, 10) }}</strong>: {{ t.amount }} руб. ({{ getItemName(t.expenseItemId) }})
        <span v-if="t.comment">— {{ t.comment }}</span>
        <button @click="deleteTransaction(t.id)">Удалить</button>
      </li>
    </ul>
    <div v-if="transactions.length === 0">
      Нет транзакций для отображения.
    </div>
  </div>
</template>

<script>
import { getTransactions, createTransaction, deleteTransaction, getTransactionsByDay, getTransactionsByMonth, getItems } from '../api.js'

export default {
  data() {
    return {
      transactions: [],
      items: [],
      newTransaction: { date: '', amount: 0, comment: '', expenseItemId: '' },
      showDay: false,
      showMonth: false,
      selectedDate: '',
      selectedYear: new Date().getFullYear(),
      selectedMonth: new Date().getMonth() + 1
    }
  },
  methods: {
    fetchAll() {
      getTransactions().then(res => this.transactions = res.data);
      getItems().then(res => this.items = res.data);
    },
    fetchByDay() {
      if (this.selectedDate)
        getTransactionsByDay(this.selectedDate).then(res => this.transactions = res.data);
    },
    fetchByMonth() {
      if (this.selectedYear && this.selectedMonth)
        getTransactionsByMonth(this.selectedYear, this.selectedMonth).then(res => this.transactions = res.data);
    },
    showAll() {
      this.showDay = false;
      this.showMonth = false;
      this.fetchAll();
    },
    addTransaction() {
      createTransaction(this.newTransaction).then(() => {
        this.fetchAll();
        this.newTransaction = { date: '', amount: 0, comment: '', expenseItemId: '' };
      });
    },
    deleteTransaction(id) {
      deleteTransaction(id).then(this.fetchAll);
    },
    getItemName(id) {
      const item = this.items.find(i => i.id === id);
      return item ? item.name : '';
    }
  },
  mounted() {
    this.fetchAll();
  }
}
</script>
