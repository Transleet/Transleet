<template>
  <q-page padding class="fit row flex-center">
    <div style="width: 300px;">
      <!-- header -->
      <div class="text-center">
        <q-icon name="account_circle" size="60px"></q-icon>
        <div class="text-opacity text-h5">{{$t('general.login.title')}}</div>
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
          <q-btn color="primary" type="submit" class="full-width q-mt-md">{{$t('general.login.login')}}</q-btn>
          <q-btn color="secondary" outline class="full-width q-mt-xs" icon="cloud_circle">{{$t('general.login.github')}}</q-btn>
        </q-form>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { useQuasar } from 'quasar';
import { ref } from 'vue';
import axios from 'axios';

const $q = useQuasar();

let isPwd = ref(true);
let email = ref('');
let password = ref('');

function submit() {
  axios.post('https://localhost:7000/authorize/token',{}).then(res => {
    $q.notify({
      message: 'Login success',
      // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
      color: res.data.success ? 'positive' : 'negative'
    });
  }).catch(err => {
    console.log(err);
  });
  $q.notify({
    color: 'positive',
    message:'success',
    timeout:3000
  });
}
</script>
