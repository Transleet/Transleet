<template>
  <q-layout view="hHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />
        <q-toolbar-title>{{ $t('general.name') }}</q-toolbar-title>
        <q-space />
        <q-badge
          rounded
          :color="
            setting.getAllSignalrState() === 'connected' ? 'green' : 'red'
          "
        />
        <div class="q-ml-xs">
          {{ $t('general.signalrState.' + setting.getAllSignalrState()) }}
        </div>
        <q-btn round class="q-ml-md" style="background-color: white">
          <q-avatar>
            <img src="https://cdn.quasar.dev/logo-v2/svg/logo.svg" alt="" />
            <q-popup-proxy>
              <avatar-helper></avatar-helper>
            </q-popup-proxy>
          </q-avatar>
        </q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-scroll-area class="fit">
        <q-list padding>
          <template v-for="m in menu" :key="m.name">
            <q-separator class="q-my-md" v-if="m.class === 'separator'" />
            <q-item-label header v-else-if="m.class === 'label'">{{
              m.name
            }}</q-item-label>
            <q-item
              :to="m.path"
              clickable
              class="flex-center"
              v-else-if="m.class === 'item'"
            >
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
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useSettingStore } from '../stores/setting';
import SignalrHubs from '../signalr/index';
import AvatarHelper from '../components/AvatarHelper.vue';
import { useCacheStore } from '../stores/cache';
// eslint-disable-next-line @typescript-eslint/unbound-method
const { t } = useI18n();
const setting = useSettingStore();
var cache = useCacheStore();
const leftDrawerOpen = ref(false);

SignalrHubs.instance.UserHub.state;

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
    class: 'label',
  },
  {
    name: t('general.drawer.home'),
    path: '/home',
    icon: 'home',
    class: 'item',
  },
];

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}

onMounted(async () => {
  while (SignalrHubs.instance.UserHub.state !== 'Connected') {
    await new Promise((resolve) => setTimeout(resolve, 100));
  }
  SignalrHubs.instance.UserHub.stream('GetAllOnlineUsers').subscribe({
    next: (u) => {
      if (u !== null) {
        cache.users.allUser.push(<string>u);
      }
    },
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    complete: () => {},
    error: (err) => {
      console.log(err);
    },
  });
});
</script>
