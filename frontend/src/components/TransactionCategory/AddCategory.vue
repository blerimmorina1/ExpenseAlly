<template>
  <div>
    <h2>Add Category</h2>
    <form @submit.prevent="addCategory">
      <label>Name:</label>
      <input v-model="name" required />

      <label>Type:</label>
      <select v-model="type" required>
        <option value="1">Income</option>
        <option value="2">Expense</option>
      </select>

      <label>Description:</label>
      <input v-model="description" />

      <button type="submit">Add</button>
    </form>
  </div>
</template>

<script>
import categoryService from '@/services/CategoryService';

export default {
  data() {
    return {
      name: '',
      description: '',
      type: 1,
    };
  },
  methods: {
    async addCategory() {
      try {
        const newCategory = {
          name: this.name,
          description: this.description,
          type: this.type,
        };
        await categoryService.createCategory(newCategory);
        this.$emit('refresh');
      } catch (error) {
        console.error('Error adding category:', error);
      }
    },
  },
};
</script>
