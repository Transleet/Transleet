import { writable } from "svelte-local-storage-store"

export const user = writable("user", {
    id:"",
    name: "",
    email: "",
    token: "",
    isLogin: false,
});