<script setup lang="ts">
import Header from "@/components/Header.vue";
import Footer from "@/components/Footer.vue";
</script>

<template>
  <div class="min-h-screen flex flex-col bg-gray-100 dark:bg-gray-600 text-gray-900 dark:text-gray-100">
    <!-- Header -->
    <Header />

    <!-- Main Content -->
    <main class="flex-grow mt-16 xs:mt-32 ms:mt-32 md:mt-16">
      <router-view />
    </main>

    <!-- Footer -->
    <Footer />
  </div>
</template>

<style>

</style>
