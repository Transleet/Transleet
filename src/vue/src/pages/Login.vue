<template>
  <q-page padding class="fit row flex-center">
    <div style="width: 300px">
      <!-- header -->
      <div class="text-center">
        <q-icon name="account_circle" size="60px"></q-icon>
        <div class="text-opacity text-h5">{{ $t('general.login.login') }}</div>
      </div>
      <!-- form -->
      <div>
        <q-form @submit="submit" class="q-mt-xl">
          <q-input
            v-model="email"
            outlined
            :label="$t('general.login.email')"
            type="email"
          ></q-input>
          <q-input
            v-model="password"
            outlined
            :type="isPwd ? 'password' : 'text'"
            :label="$t('general.login.password')"
            class="q-mt-md"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>
          <q-btn color="primary" type="submit" class="full-width q-mt-md">{{
            $t('general.login.login')
          }}</q-btn>
          <q-btn
            color="secondary"
            outline
            class="full-width q-mt-xs"
            icon="cloud_circle"
            >{{ $t('general.login.github') }}</q-btn
          >
        </q-form>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useQuasar } from 'quasar';
import { ref } from 'vue';
import axios from 'axios';
import { useI18n } from 'vue-i18n';
import { useSettingStore } from '../store/setting';

const $q = useQuasar();
// eslint-disable-next-line @typescript-eslint/unbound-method
const { t } = useI18n();
const setting = useSettingStore();

let isPwd = ref(true);
let email = ref('');
let password = ref('');

function submit() {
  if (email.value === '' || password.value === '') {
    $q.notify({
      message: t('general.login.valueCannotBeNull'),
      color: 'negative',
      timeout: 1500,
    });
    return;
  }
  axios
    .post(
      // eslint-disable-next-line @typescript-eslint/restrict-plus-operands
      'https://localhost:7000/api' + '/authorize/token',
      {
        inputText: email.value,
        password: password.value,
      },
      {
        headers: {
          'Access-Control-Allow-Origin': '*',
        },
      }
    )
    .then((res) => {
      $q.notify({
        message: t('general.login.loginSuccess'),
        color: 'positive',
        timeout: 1500,
      });
      // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
      setting.token = JSON.parse(JSON.stringify(res.data)).token;
    })
    .catch((err) => {
      console.log(err);
      $q.notify({
        // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
        message: err.message,
        color: 'negative',
        timeout: 1500,
      });
    });
}
</script>
