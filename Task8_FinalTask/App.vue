<template>
  <div id="app">
    <div class="header">Личный бюджет</div>
    <div class="tabs">
      <button :class="['tab-btn', activeTab === 'categories' ? 'active' : '']" @click="activeTab = 'categories'">Категории</button>
      <button :class="['tab-btn', activeTab === 'items' ? 'active' : '']" @click="activeTab = 'items'">Статьи</button>
      <button :class="['tab-btn', activeTab === 'transactions' ? 'active' : '']" @click="activeTab = 'transactions'">Транзакции</button>
    </div>

    <!-- Категории -->
    <div v-if="activeTab === 'categories'">
      <h2>Категории расходов</h2>
      <form @submit.prevent="addCategory">
        <input v-model="newCategory.name" placeholder="Название" required>
        <input v-model.number="newCategory.monthlyBudget" type="number" placeholder="Бюджет" required>
        <label><input type="checkbox" v-model="newCategory.isActive"> Активна</label>
        <button type="submit">Добавить</button>
      </form>
      <ul>
        <li v-for="cat in categories" :key="cat.id">
          <strong>{{ cat.name }}</strong> ({{ cat.monthlyBudget }} руб.)
          <span v-if="cat.isActive" style="color:green;">[Активна]</span>
          <span v-else style="color:gray;">[Неактивна]</span>
          <button @click="editCategory(cat)">Редактировать</button>
          <button @click="deleteCategory(cat.id)">Удалить</button>
        </li>
      </ul>
      <div v-if="categories.length === 0">
        Нет категорий для отображения.
      </div>
      <div v-if="editingCategory">
        <h3>Редактировать категорию</h3>
        <form @submit.prevent="updateCategory">
          <input v-model="editingCategory.name" placeholder="Название" required>
          <input v-model.number="editingCategory.monthlyBudget" type="number" placeholder="Бюджет" required>
          <label><input type="checkbox" v-model="editingCategory.isActive"> Активна</label>
          <button type="submit">Сохранить</button>
          <button @click="cancelEditCategory" type="button">Отмена</button>
        </form>
      </div>
    </div>

    <!-- Статьи -->
    <div v-if="activeTab === 'items'">
      <h2>Статьи расходов</h2>
      <form @submit.prevent="addItem">
        <input v-model="newItem.name" placeholder="Название" required>
        <select v-model.number="newItem.categoryId" required>
          <option disabled value="">Выберите категорию</option>
          <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
        </select>
        <label><input type="checkbox" v-model="newItem.isActive"> Активна</label>
        <button type="submit">Добавить</button>
      </form>
      <ul>
        <li v-for="item in items" :key="item.id">
          <strong>{{ item.name }}</strong> ({{ getCategoryName(item.categoryId) }})
          <span v-if="item.isActive" style="color:green;">[Активна]</span>
          <span v-else style="color:gray;">[Неактивна]</span>
          <button @click="editItem(item)">Редактировать</button>
          <button @click="deleteItem(item.id)">Удалить</button>
        </li>
      </ul>
      <div v-if="items.length === 0">
        Нет статей для отображения.
      </div>
      <div v-if="editingItem">
        <h3>Редактировать статью</h3>
        <form @submit.prevent="updateItem">
          <input v-model="editingItem.name" placeholder="Название" required>
          <select v-model.number="editingItem.categoryId" required>
            <option disabled value="">Выберите категорию</option>
            <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
          </select>
          <label><input type="checkbox" v-model="editingItem.isActive"> Активна</label>
          <button type="submit">Сохранить</button>
          <button @click="cancelEditItem" type="button">Отмена</button>
        </form>
      </div>
    </div>

    <!-- Транзакции -->
    <div v-if="activeTab === 'transactions'">
      <h2>Транзакции</h2>
      <form @submit.prevent="addTransaction">
        <input v-model="newTransaction.date" type="date" required>
        <input v-model.number="newTransaction.amount" type="number" placeholder="Сумма" required>
        <input v-model="newTransaction.comment" placeholder="Комментарий">
        <select v-model.number="newTransaction.expenseItemId" required>
          <option disabled value="">Выберите статью</option>
          <option v-for="item in activeItems" :key="item.id" :value="item.id">{{ item.name }}</option>
        </select>
        <button type="submit">Добавить</button>
      </form>
      <div class="filters">
        <button @click="showAll">Все</button>
        <button @click="showDay = !showDay">За день</button>
        <button @click="showMonth = !showMonth">За месяц</button>
      </div>
      <div v-if="showDay">
        <input type="date" v-model="selectedDate" @change="fetchByDay" />
        <div v-if="daySticker" style="margin-top:10px; font-weight:bold;">
          {{ daySticker }}
        </div>
      </div>
      <div v-if="showMonth">
        <input type="number" v-model="selectedYear" min="2000" max="2100" placeholder="Год" @change="fetchByMonth" />
        <input type="number" v-model="selectedMonth" min="1" max="12" placeholder="Месяц" @change="fetchByMonth" />
      </div>
      <ul>
        <li v-for="t in transactions" :key="t.id">
          <strong>{{ t.date.slice(0, 10) }}</strong>: {{ t.amount }} руб. ({{ getItemName(t.expenseItemId) }})
          <span v-if="t.comment">— {{ t.comment }}</span>
          <button @click="editTransaction(t)">Редактировать</button>
          <button @click="deleteTransaction(t.id)">Удалить</button>
        </li>
      </ul>
      <div v-if="transactions.length === 0">
        Нет транзакций для отображения.
      </div>
      <div v-if="editingTransaction">
        <h3>Редактировать транзакцию</h3>
        <form @submit.prevent="updateTransaction">
          <input v-model="editingTransaction.date" type="date" required>
          <input v-model.number="editingTransaction.amount" type="number" placeholder="Сумма" required>
          <input v-model="editingTransaction.comment" placeholder="Комментарий">
          <select
            v-model.number="editingTransaction.expenseItemId"
            :disabled="!getItemById(editingTransaction.expenseItemId)?.isActive"
            required
          >
            <option disabled value="">Выберите статью</option>
            <option
              v-for="item in selectableItems"
              :key="item.id"
              :value="item.id"
            >{{ item.name }}</option>
          </select>
          <button type="submit">Сохранить</button>
          <button @click="cancelEditTransaction" type="button">Отмена</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import {
  getCategories, createCategory, updateCategory, deleteCategory,
  getItems, createItem, updateItem, deleteItem,
  getTransactions, createTransaction, updateTransaction, deleteTransaction,
  getTransactionsByDay, getTransactionsByMonth, getDaySticker
} from './api.js'

export default {
  data() {
    return {
      activeTab: 'categories',
      categories: [],
      items: [],
      transactions: [],
      newCategory: { name: '', monthlyBudget: 0, isActive: true },
      newItem: { name: '', categoryId: '', isActive: true },
      newTransaction: { date: '', amount: 0, comment: '', expenseItemId: '' },
      editingCategory: null,
      editingItem: null,
      editingTransaction: null,
      showDay: false,
      showMonth: false,
      selectedDate: '',
      selectedYear: new Date().getFullYear(),
      selectedMonth: new Date().getMonth() + 1,
      daySticker: ''
    }
  },
  computed: {
    activeItems() {
      return this.items.filter(item => item.isActive);
    },
    selectableItems() {
      // Для редактирования: только активные или выбраная неактивная
      if (!this.editingTransaction) {
        return this.items.filter(i => i.isActive);
      }
      return this.items.filter(
        i => i.isActive || i.id === this.editingTransaction.expenseItemId
      );
    }
  },
  methods: {
    refreshCategories() {
      getCategories().then(res => this.categories = res.data);
    },
    addCategory() {
      createCategory(this.newCategory).then(() => {
        this.refreshCategories();
        this.newCategory = { name: '', monthlyBudget: 0, isActive: true };
      });
    },
    editCategory(cat) {
      this.editingCategory = { ...cat };
    },
    updateCategory() {
      updateCategory(this.editingCategory.id, this.editingCategory).then(() => {
        this.refreshCategories();
        this.editingCategory = null;
      });
    },
    cancelEditCategory() {
      this.editingCategory = null;
    },
    deleteCategory(id) {
      deleteCategory(id).then(this.refreshCategories);
    },

    refreshItems() {
      getItems().then(res => this.items = res.data);
    },
    addItem() {
      createItem(this.newItem).then(() => {
        this.refreshItems();
        this.newItem = { name: '', categoryId: '', isActive: true };
      });
    },
    editItem(item) {
      this.editingItem = { ...item };
    },
    updateItem() {
      updateItem(this.editingItem.id, this.editingItem).then(() => {
        this.refreshItems();
        this.editingItem = null;
      });
    },
    cancelEditItem() {
      this.editingItem = null;
    },
    deleteItem(id) {
      deleteItem(id).then(this.refreshItems);
    },
    getCategoryName(id) {
      const cat = this.categories.find(c => c.id === id);
      return cat ? cat.name : '';
    },

    refreshTransactions() {
      getTransactions().then(res => this.transactions = res.data);
    },

    // ТЕХНИЧЕСКОЕ ОГРАНИЧЕНИЕ по СУММЕ ЗА ДЕНЬ
    addTransaction() {
      const dateStr = this.isoDate(this.newTransaction.date);
      const sumForDay = this.transactions
        .filter(t => (t.date && t.date.slice(0, 10)) === dateStr)
        .reduce((sum, t) => sum + Number(t.amount), 0);

      const newTotal = sumForDay + Number(this.newTransaction.amount);
      if (newTotal > 1000000) {
        alert('Нельзя добавить: суммарная сумма транзакций за день превышает 1 000 000 рублей!');
        return;
      }
      createTransaction(this.newTransaction).then(() => {
        this.refreshTransactions();
        this.newTransaction = { date: '', amount: 0, comment: '', expenseItemId: '' };
      });
    },

    editTransaction(t) {
      this.editingTransaction = { ...t };
    },
    updateTransaction() {
      updateTransaction(this.editingTransaction.id, this.editingTransaction).then(() => {
        this.refreshTransactions();
        this.editingTransaction = null;
      });
    },
    cancelEditTransaction() {
      this.editingTransaction = null;
    },
    deleteTransaction(id) {
      deleteTransaction(id).then(this.refreshTransactions);
    },

    getItemName(id) {
      const item = this.items.find(i => i.id === id);
      return item ? item.name : '';
    },
    getItemById(id) {
      return this.items.find(i => i.id === id);
    },

    showAll() {
      this.showDay = false;
      this.showMonth = false;
      this.refreshTransactions();
    },
    fetchByDay() {
      if (!this.selectedDate) return;
      let dateStr = this.selectedDate;
      if (/^\d{2}\.\d{2}\.\d{4}$/.test(dateStr)) {
        const [d, m, y] = dateStr.split('.');
        dateStr = `${y}-${m}-${d}`;
      }
      getTransactionsByDay(dateStr).then(res => this.transactions = res.data);
      getDaySticker(dateStr)
        .then(res => {
          this.daySticker = res.data;
        })
        .catch(() => {
          this.daySticker = '';
        });
    },
    fetchByMonth() {
      if (this.selectedYear && this.selectedMonth)
        getTransactionsByMonth(this.selectedYear, this.selectedMonth).then(res => this.transactions = res.data);
    },
    isoDate(str) {
      if (!str) return '';
      if (/^\d{4}-\d{2}-\d{2}$/.test(str)) return str;
      if (/^\d{2}\.\d{2}\.\d{4}$/.test(str)) {
        const [d, m, y] = str.split('.');
        return `${y}-${m}-${d}`;
      }
      return str;
    }
  },
  mounted() {
    this.refreshCategories();
    this.refreshItems();
    this.refreshTransactions();
  }
}
</script>

<style scoped>
.header {
  background: #2ecc40;
  color: #fff;
  font-size: 2.2em;
  font-weight: bold;
  padding: 24px 0 18px 0;
  margin-bottom: 30px;
  letter-spacing: 2px;
}
.tabs {
  margin-bottom: 30px;
}
.tab-btn {
  background: #3b82f6;
  color: #fff;
  border: none;
  padding: 12px 28px;
  margin: 0 8px 0 0;
  font-size: 1.1em;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.2s;
}
.tab-btn:last-child {
  background: #8e44ad;
}
.tab-btn.active {
  box-shadow: 0 0 0 3px #b2f7c1;
  font-weight: bold;
  outline: none;
}
.tab-btn:hover {
  opacity: 0.85;
}
form {
  margin-bottom: 20px;
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
.filters {
  margin-bottom: 10px;
}
</style>