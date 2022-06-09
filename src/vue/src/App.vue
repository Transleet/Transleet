<template>
  <router-view />
</template>
<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'App',
});
</script>

<script setup lang="ts">
import { useSettingStore } from './stores/setting';
import { Dark } from 'quasar';
const setting = useSettingStore();
Dark.set(setting.dark)
</script>
