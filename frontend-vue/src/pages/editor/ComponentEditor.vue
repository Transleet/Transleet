<template>
  <q-page padding style="padding-top: 60px" class="q-gutter-md">
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <edit-component></edit-component>
    <q-page-sticky expand position="top">
      <q-toolbar class="">
        <q-space />
        <q-input
          dense
          rounded
          outlined
          v-model="search"
          input-class="text-right"
          class="q-ml-md"
        >
          <template v-slot:append>
            <q-icon v-if="search === ''" name="search" />
            <q-icon
              v-else
              name="clear"
              class="cursor-pointer"
              @click="search = ''"
            />
          </template>
        </q-input>
        <q-space />
      </q-toolbar>
    </q-page-sticky>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import EditComponent from 'src/components/editor/EditComponent.vue';

let search = ref('');
console.log(search);
</script>

<style lang="css">
.search {
  position: sticky;
  position: -webkit-sticky;
  top: 0.64rem;
}
</style>
