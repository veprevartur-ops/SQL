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
        <button @click="deleteCategory(cat.id)">Удалить</button>
      </li>
    </ul>
    <div v-if="categories.length === 0">
      Нет категорий для отображения.
    </div>
  </div>
</template>

<script>
import { getCategories, createCategory, deleteCategory } from '../api.js'

export default {
  data() {
    return {
      categories: [],
      newCategory: { name: '', monthlyBudget: 0, isActive: true }
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
    deleteCategory(id) {
      deleteCategory(id).then(this.refresh);
    }
  },
  mounted() {
    this.refresh();
  }
}
</script>