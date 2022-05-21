<template>
<div class="q-gutter-md">
  <q-card>
    <q-card-section class="bg-primary">
    </q-card-section>
    <q-card-section></q-card-section>
  </q-card></div>
</template>

<script setup lang="ts">
import { useCacheStore } from '../../store/cache';
import { ComponentsService } from '../../lib/api/services/ComponentsService';
let cache = useCacheStore();
</script>
