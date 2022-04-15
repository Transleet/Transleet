import { Project } from '../models/Project';
<template>
  <q-card style="width: 192px;">
    <q-card-section>
      <q-img
        width="160px"
        :src="props.project.avatar"
        :alt="props.project.displayName"
        ratio="1"
      ></q-img>
      <div style="font-size: 14px; font-weight: bold;overflow: hidden;">{{ props.project.displayName }}</div>
      <div style="font-size: 13px; overflow: hidden;">{{ props.project.description }}</div>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { Project } from 'src/models/Project';

const props = defineProps<{project:Project}>();
</script>

<style lang="scss">

</style>
