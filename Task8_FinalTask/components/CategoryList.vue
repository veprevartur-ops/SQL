<template>
  <div>
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
        <!-- Убрано: <button @click="viewCategory(cat.id)">Посмотреть</button> -->
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
</template>

<script>
import { getCategories, createCategory, deleteCategory, updateCategory } from '../api.js'

export default {
  data() {
    return {
      categories: [],
      newCategory: { name: '', monthlyBudget: 0, isActive: true },
      editingCategory: null
    }
  },
  methods: {
    refresh() {
      getCategories().then(res => this.categories = res.data);
    },
    addCategory() {
      createCategory(this.newCategory).then(() => {
        this.refresh();
        this.newCategory = { name: '', monthlyBudget: 0, isActive: true };
      });
    },
    editCategory(cat) {
      this.editingCategory = { ...cat };
    },
    updateCategory() {
      updateCategory(this.editingCategory.id, this.editingCategory).then(() => {
        this.refresh();
        this.editingCategory = null;
      });
    },
    cancelEditCategory() {
      this.editingCategory = null;
    },
    deleteCategory(id) {
      deleteCategory(id).then(this.refresh);
    }
  },
  mounted() {
    this.refresh();
  }
}
</script>