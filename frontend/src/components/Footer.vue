<script setup lang="ts">
const currentYear = new Date().getFullYear();
</script>

<template>
  <footer
    class="py-4 text-black transition-colors bg-gray-200"
  >
    <div class="mx-auto text-center">
      <p class="text-sm">
        &copy; {{ currentYear }} <span class="font-semibold">ExpenseAlly</span>. All rights reserved.
      </p>
    </div>
  </footer>
</template>

<style scoped>

</style>
