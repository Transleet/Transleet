<template>
  <q-page padding class="fit row flex-center">
    <div style="width: 300px">
      <!-- header -->
      <div class="text-center">
        <q-icon name="account_circle" size="60px"></q-icon>
        <div class="text-opacity text-h5">
          {{ $t('general.login.register') }}
        </div>
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
            v-model="username"
            outlined
            :label="$t('general.login.username')"
            class="q-mt-md"
          ></q-input>
          <q-input
            v-model="password1"
            outlined
            :label="$t('general.login.password')"
            class="q-mt-md"
            type="password"
          ></q-input>
          <q-input
            v-model="password2"
            outlined
            :label="$t('general.login.password2')"
            class="q-mt-md"
            type="password"
            :hint="
              password1 === password2
                ? ''
                : $t('general.login.passwordNotMatch')
            "
          ></q-input>
          <q-btn color="primary" type="submit" class="full-width q-mt-md">
            {{ $t('general.login.register') }}
          </q-btn>
        </q-form>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import axios from 'axios';
import { useQuasar } from 'quasar';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useSettingStore } from '../store/setting';

const $q = useQuasar();
const setting = useSettingStore();
// eslint-disable-next-line @typescript-eslint/unbound-method
const { t } = useI18n();

let email = ref('');
let username = ref('');
let password1 = ref('');
let password2 = ref('');

async function submit() {
  if (password1.value !== password2.value) {
    $q.notify({
      message: t('general.login.passwordNotMatch'),
      color: 'negative',
      timeout: 1500,
    });
  }
  await axios
    .post(
      // eslint-disable-next-line @typescript-eslint/restrict-plus-operands
      'https://localhost:7000/api' + '/account/register',
      {
        username: username.value,
        email: email.value,
        password: password1.value,
      },
      {
        headers: {
          'Access-Control-Allow-Origin': '*',
        },
      }
    )
    .then((res) => {
      axios
        .post(
          'https://localhost:7000/api/authorize/token',
          {
            // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
            inputText: res.data.email,
            password: password1.value,
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
