<script setup lang="ts">
const version = 2 + 1
const { data: message } = await useAsyncData(
    'message',
    (_nuxtApp, { signal }) => $fetch('http://localhost:5076/api/home', { signal }),
)
</script>

<template>
  <v-app>
    <div>
      <p class="hello">
        Hello Nuxt {{ version }}!
      </p>
      <p class="hello">
        {{ message }}
      </p>
    </div>
  </v-app>
</template>

<style scoped>
.hello {
  font-family: Arial, Helvetica, sans-serif;
  font-size: 3rem;
  padding: 2rem;
}
</style>