<template>
  <q-card>
    <q-card-section>
      <div class="text-subtitle1">eng value</div>
      <div class="row-md justify-between items-center">
        <div class="text-subtitle2">key</div>
        <div class="row-md items-center">
          <div class="q-mr-md">lang</div>
          <q-chip icon="done_all" color="primary" text-color="white">state</q-chip>
        </div>
      </div>
    </q-card-section>
    <q-separator />
    <q-card-section>
      <q-input type="textarea" v-model="translation" outlined autogrow dense />
      <q-expansion-item expand-separator icon="format_quote" label="suggest" class="q-mt-xs">
      <p>114</p>
      <p>114</p>
      <p>114</p>
      <p>114</p>
      <p>114</p>
      </q-expansion-item>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';

let translation = ref('');
</script>
