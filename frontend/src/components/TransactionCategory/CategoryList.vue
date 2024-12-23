<template>
  <div>
    <h1>Transaction Categories</h1>
    <button @click="$emit('add')">Add New Category</button>
    <ul>
      <li v-for="category in categories" :key="category.id">
        <strong>{{ category.name }}</strong> ({{ category.type }}) - {{ category.description }}
        <button @click="$emit('edit', category)">Edit</button>
        <button @click="deleteCategory(category.id)">Delete</button>
      </li>
    </ul>
  </div>
</template>

<script>
import categoryService from '@/services/CategoryService';

export default {
  props: ['categories'],
  methods: {
    async deleteCategory(id) {
      try {
        await categoryService.deleteCategory(id);
        this.$emit('refresh');
      } catch (error) {
        console.error('Error deleting category:', error);
      }
    },
  },
};
</script>
