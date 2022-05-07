<script lang="ts">
	import { goto } from '$app/navigation';
	import { user } from '$lib/stores';
	import axios from 'axios';
	let inputText: string;
	let password: string;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	async function login() {
		let loginResponse = await axios.post(backend_base_url + '/api/authorize/token', {
			inputText: inputText,
			password: password
		});
		$user.token = loginResponse.data.token;
	}
	async function githubLogin() {
		goto(
			backend_base_url +
				'/api/oauth/github_login?returnUrl=' +
				frontend_base_url +
				'/github_login_callback'
		);
	}
</script>

<div>
	<div class="flex flex-col items-center">
		<div class="w-full max-w-xs">
			<label class="label">
				<span class="label-text">Username/Email</span>
			</label>
			<input
				type="text"
				bind:value={inputText}
				placeholder="Your username/email."
				class="input input-bordered w-full max-w-xs" />
			<label class="label">
				<span class="label-text">Password</span>
			</label>
			<input
				type="password"
				bind:value={password}
				placeholder="Your password."
				class="input input-bordered w-full max-w-xs" />
			<div class="flex flex-row justify-around pt-2">
				<button
					class="btn"
					on:click={() => {
						goto('/register');
					}}>Register</button>
				<button class="btn" on:click={login}>Login</button>
				<button class="btn" on:click={githubLogin}>Login Via Github</button>
			</div>
		</div>
	</div>
</div>
