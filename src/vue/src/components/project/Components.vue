<template>
<div class="q-gutter-md">
  <q-card>
    <q-card-section class="bg-primary">
    </q-card-section>
    <q-card-section></q-card-section>
  </q-card></div>
</template>

<script setup lang="ts">
import { useCacheStore } from '../../stores/cache';
import { ComponentsService } from '../../lib/api/services/ComponentsService';
let cache = useCacheStore();
</script>

<script>
export default {
  name: 'component',
}
</script>
