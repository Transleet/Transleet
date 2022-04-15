<script lang="ts">
	import { goto } from '$app/navigation';
	import axios from 'axios';
	let inputText: string;
	let password: string;
	async function login() {
		let loginResponse = await axios.post('/api/authorize/token', {
			inputText: inputText,
			password: password
		});
		console.log(loginResponse);
		localStorage.setItem('token', loginResponse.data.token);
	}
	async function githubLogin() {
		goto('/api/oauth/github_login?returnUrl=/github_login_callback');
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
