import { writable } from "svelte-local-storage-store"

export const user = writable("user", {
    name: "",
    email: "",
    token: "",
    isLogin: false,
});