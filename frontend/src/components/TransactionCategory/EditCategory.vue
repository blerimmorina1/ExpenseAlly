<template>
  <div>
    <h2>Edit Category</h2>
    <form @submit.prevent="updateCategory">
      <label>Name:</label>
      <input v-model="category.name" required />

      <label>Type:</label>
      <select v-model="category.type" required>
        <option value="1">Income</option>
        <option value="2">Expense</option>
      </select>

      <label>Description:</label>
      <input v-model="category.description" />

      <button type="submit">Save</button>
    </form>
  </div>
</template>

<script>
import categoryService from '@/service/categoryService';

export default {
  props: ['category'],
  methods: {
    async updateCategory() {
      try {
        await categoryService.updateCategory(this.category);
        this.$emit('refresh');
      } catch (error) {
        console.error('Error updating category:', error);
      }
    },
  },
};
</script>
