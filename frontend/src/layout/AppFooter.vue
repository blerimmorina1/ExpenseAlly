<script setup lang="ts">
const currentYear = new Date().getFullYear();
</script>

<template>
    <div class="layout-footer">
      <div class="mx-auto text-center">
        <p class="text-sm">
          &copy; {{ currentYear }} <span class="font-semibold">ExpenseAlly</span>. All rights reserved.
        </p>
      </div>
    </div>
</template>

<style scoped>

</style>
