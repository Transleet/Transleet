import { writable } from "svelte-local-storage-store"

export const userStore = writable("user", {
    id:"",
    token: "",
    isLogin: false,
});