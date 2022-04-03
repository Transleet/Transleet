<template>
  <q-layout view="hHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />
        <q-toolbar-title>{{ $t('general.name') }}</q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-scroll-area class="fit">
        <q-list padding>
          <template v-for="m in menu" :key="m.name">
            <q-separator class="q-my-md" v-if="m.class === 'separator'" />
            <q-item-label header v-else-if="m.class === 'label'">{{ m.name }}</q-item-label>
            <q-item :to="m.path" clickable class="flex-center" v-else-if="m.class === 'item'">
              <q-item-section avatar>
                <q-icon :name="m.icon"></q-icon>
              </q-item-section>
              <q-item-section>
                <q-item-label>{{ m.name }}</q-item-label>
              </q-item-section>
            </q-item>
          </template>
        </q-list>
      </q-scroll-area>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';

// eslint-disable-next-line @typescript-eslint/unbound-method
const { t } = useI18n();
const leftDrawerOpen = ref(false);

interface MenuOption {
  name: string;
  path: string;
  icon: string;
  class: string;
}

const menu: Array<MenuOption> = [
  {
    name: t('general.drawer.commonuse'),
    path: '',
    icon: '',
    class: 'label'
  },
  {
    name: t('general.drawer.home'),
    path: '/home',
    icon: 'home',
    class: 'item'
  },
  {
    name: 'Login',
    path: '/login',
    icon: 'mail',
    class: 'item',
  },
];

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}
</script>
