<script>
	import { goto } from '$app/navigation';
	import axios from 'axios';
	async function githubLogin() {
		goto('/api/oauth/github_login?returnUrl=/github_login_callback');
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
