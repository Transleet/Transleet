import { Project } from '../models/Project';
<template>
  <q-card style="width: 192px;">
    <q-card-section>
      <q-img
        width="160px"
        :src="props.project.avatar + ''"
        :alt="props.project.displayName + ''"
        ratio="1"
      ></q-img>
      <div style="font-size: 16px; overflow: hidden;" class="q-mt-xs">{{ props.project.name }}</div>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { Project } from 'src/lib/api';

const props = defineProps<{project:Project}>();
</script>

<style lang="scss">

</style>
