<script lang="ts">
	import { goto } from '$app/navigation';
	import { user } from '$lib/stores';

	import axios from 'axios';
	let username: string;
	let email: string;
	let password: string;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	async function register() {
		let registerResponse = await axios.post(backend_base_url + '/api/account/register', {
			username: username,
			email: email,
			password: password
		});
		let loginResponse = await axios.post(backend_base_url + '/api/authorize/token', {
			inputText: registerResponse.data.email,
			password: password
		});
		console.log(loginResponse);
		$user.token = loginResponse.data.token;
		goto(frontend_base_url + '/register_confirm');
	}
</script>

<div>
	<div class="flex flex-col items-center">
		<div class="w-full max-w-xs">
			<label class="label">
				<span class="label-text">Username</span>
			</label>
			<input
				type="text"
				bind:value={username}
				placeholder="Your username."
				class="input input-bordered w-full max-w-xs" />
			<label class="label">
				<span class="label-text">Email</span>
			</label>
			<input
				type="email"
				bind:value={email}
				placeholder="Your email."
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
				<button class="btn" on:click={register}>Register</button>
			</div>
		</div>
	</div>
</div>
