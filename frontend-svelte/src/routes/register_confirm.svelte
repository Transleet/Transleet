<script>
	import { goto } from '$app/navigation';
	import axios from 'axios';
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	async function githubLogin() {
		goto(
			backend_base_url + '/api/oauth/github_login?returnUrl=' + frontend_base_url +'/github_login_callback'
		);
	}
</script>

<div class="flex flex-col items-center">
	<p>You can choose the way to vertify your email.</p>
	<p>Via a code in your email or by github.</p>
	<div class="flex flex-row pt-4">
		<button class="btn">Confirm email</button>
		<button class="btn" on:click={githubLogin}>Confirm via github</button>
	</div>
</div>
