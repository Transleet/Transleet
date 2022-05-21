<script lang="ts">
	import 'virtual:windi.css';
	import { goto } from '$app/navigation';
	import { userStore } from '$lib/stores';
	import { onMount } from 'svelte';
	import { UsersService, type User } from '$lib/api';
	let user: User;

	function logout() {
		$userStore = {
			token: '',
			id: '',
			isLogin: false
		};
		user = null;
	}

	onMount(async () => {
		if ($userStore.isLogin) {
			user = await UsersService.getUserById($userStore.id);
		}
	});
</script>

<header>
	<div class="navbar bg-base-100">
		<div class="flex-1">
			<a href="/" class="btn btn-ghost normal-case text-xl">Transleet</a>
		</div>
		<div class="flex-none gap-2">
			<div class="form-control">
				<input type="text" placeholder="Search" class="input input-bordered" />
			</div>
			<div class="dropdown dropdown-end">
				{#if $userStore.isLogin}
					{#if user}
						<div tabindex="0" class="btn btn-ghost btn-circle avatar">
							<div class="w-10 rounded-full">
								<img src={user.avatarUrl ?? '/no_image.jpg'} alt="avatar" />
							</div>
						</div>
						<ul
							tabindex="0"
							class="p-2 shadow menu menu-compact dropdown-content bg-base-100 rounded-box w-52">
							<li>
								<a class="justify-between">
									Profile
									<span class="badge">New</span>
								</a>
							</li>
							<li><a>Settings</a></li>
							<li><a on:click={logout}>Logout</a></li>
						</ul>
					{/if}
				{:else}
					<button class="btn normal-case" on:click={() => goto('/login')}>Login</button>
				{/if}
			</div>
		</div>
	</div>
</header>
<main>
	<slot />
</main>
<footer />
