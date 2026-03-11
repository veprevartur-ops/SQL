<template>
  <div>
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
        <button @click="deleteItem(item.id)">Удалить</button>
      </li>
    </ul>
    <div v-if="items.length === 0">
      Нет статей для отображения.
    </div>
  </div>
</template>

<script>
import { getItems, createItem, deleteItem, getCategories } from '../api.js'

export default {
  data() {
    return {
      items: [],
      categories: [],
      newItem: { name: '', categoryId: '', isActive: true }
    }
  },
  methods: {
    refresh() {
      getItems().then(res => this.items = res.data);
      getCategories().then(res => this.categories = res.data);
    },
    addItem() {
      createItem(this.newItem).then(() => {
        this.refresh();
        this.newItem = { name: '', categoryId: '', isActive: true };
      });
    },
    deleteItem(id) {
      deleteItem(id).then(this.refresh);
    },
    getCategoryName(id) {
      const cat = this.categories.find(c => c.id === id);
      return cat ? cat.name : '';
    }
  },
  mounted() {
    this.refresh();
  }
}
</script>