<template>
  <div>
    <h2>Транзакции</h2>

    <!-- Сообщение для пользователя -->
    <div v-if="userMessage" :style="messageStyle">{{ userMessage }}</div>

    <div class="filters">
      <button @click="showAll">Все</button>
      <button @click="toggleDay">За день</button>
      <button @click="toggleMonth">За месяц</button>
    </div>
    <div v-if="showDay">
      <input
        type="date"
        v-model="selectedDate"
        @change="fetchByDay"
        placeholder="ГГГГ-ММ-ДД"
      />
      <!-- Стикер расходов -->
      <div v-if="daySticker" :style="stickerStyle">{{ daySticker }}</div>
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
import { 
  getTransactions, createTransaction, deleteTransaction, 
  getTransactionsByDay, getTransactionsByMonth, getItems, getDaySticker 
} from '../api.js'

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
      selectedMonth: new Date().getMonth() + 1,
      userMessage: '',
      daySticker: ''
    }
  },
  computed: {
    messageStyle() {
      return {
        margin: "15px 0",
        color: "#ac2925",
        background: "#f2dede",
        border: "1px solid #ebcccc",
        padding: "8px",
        fontWeight: "bold",
        borderRadius: "6px"
      }
    },
    stickerStyle() {
      let color = "#222";
      if (this.daySticker.startsWith('🟩')) color = "green";
      else if (this.daySticker.startsWith('🟨')) color = "#c09853";
      else if (this.daySticker.startsWith('🟥')) color = "#b94a48";
      return {
        fontSize: "1.15em",
        fontWeight: "bold",
        margin: "9px 0",
        color
      }
    }
  },
  methods: {
    fetchAll() {
      getTransactions().then(res => this.transactions = res.data);
      getItems().then(res => this.items = res.data);
      this.daySticker = '';
    },
    fetchByDay() {
  if (!this.selectedDate) return;
  let dateStr = this.selectedDate;

  // Обработка dd.MM.yyyy ➔ yyyy-MM-dd
  if (/^\d{2}\.\d{2}\.\d{4}$/.test(dateStr)) {
    const [d, m, y] = dateStr.split('.');
    dateStr = `${y}-${m}-${d}`;
  }
  // Обработка MM/dd/yyyy ➔ yyyy-MM-dd (на всякий случай)
  else if (/^\d{2}\/\d{2}\/\d{4}$/.test(dateStr)) {
    const [m, d, y] = dateStr.split('/');
    dateStr = `${y}-${m}-${d}`;
  }
  // Если ISO — оставляем
  else if (/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
    // всё хорошо!
  } else {
    // неизвестный формат
    this.userMessage = 'Некорректный формат даты!';
    return;
  }

  console.log("Отправляю дату:", dateStr);

  getTransactionsByDay(dateStr).then(res => this.transactions = res.data);
  getDaySticker(dateStr)
    .then(res => {
      this.daySticker = res.data;
      console.log("Пришел стикер:", res.data);
    })
    .catch(err => {
      this.daySticker = '';
      this.userMessage = 'Ошибка получения стикера!';
      console.log("Ошибка стикера:", err);
    });
}
    fetchByMonth() {
      if (this.selectedYear && this.selectedMonth)
        getTransactionsByMonth(this.selectedYear, this.selectedMonth).then(res => this.transactions = res.data);
      this.daySticker = '';
    },
    showAll() {
      this.userMessage = '';
      this.showDay = false;
      this.showMonth = false;
      this.daySticker = '';
      this.fetchAll();
    },
    toggleDay() {
      this.showDay = !this.showDay;
      if (this.showDay) {
        this.showMonth = false;
        this.daySticker = '';
        if (this.selectedDate) this.fetchByDay();
      }
    },
    toggleMonth() {
      this.showMonth = !this.showMonth;
      if (this.showMonth) {
        this.showDay = false;
        this.daySticker = '';
        this.fetchByMonth();
      }
    },
    addTransaction() {
      this.userMessage = '';
      let tx = { ...this.newTransaction };
      // <input type="date"> возвращает ISO, так что можно отправлять как есть
      tx.date = tx.date.length > 10 ? tx.date.slice(0, 10) : tx.date;
      createTransaction(tx)
        .then(() => {
          this.fetchAll();
          this.newTransaction = { date: '', amount: 0, comment: '', expenseItemId: '' };
          this.userMessage = 'Транзакция успешно добавлена!';
        })
        .catch(err => {
          if (err.response && err.response.data) {
            this.userMessage = err.response.data;
          } else {
            this.userMessage = 'Ошибка при добавлении транзакции';
          }
        });
    },
    deleteTransaction(id) {
      this.userMessage = '';
      deleteTransaction(id)
        .then(() => {
          this.fetchAll();
          this.userMessage = 'Транзакция удалена';
        })
        .catch(() => {
          this.userMessage = 'Ошибка при удалении транзакции';
        });
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

<style scoped>
.filters {
  margin-bottom: 10px;
}
.filters button {
  margin-right: 10px;
  background: #eee;
  border: 1px solid #ccc;
  padding: 7px 18px;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
  transition: background .18s;
}
.filters button:hover {
  background: #e2e2e2;
}
form {
  margin-bottom: 20px;
  background: #fafafa;
  padding: 10px;
  border-radius: 6px;
}
input, select {
  margin: 0 8px 8px 0;
  padding: 6px;
  border-radius: 4px;
  border: 1px solid #ccc;
}
button {
  margin-left: 8px;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  margin-bottom: 8px;
}
</style>