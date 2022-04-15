<script lang="ts">
	import 'virtual:windi.css';
	import { page } from '$app/stores';
	import { base, assets } from '$app/paths';
	import { goto } from '$app/navigation';
	let avatarUrl;
	let islogin: boolean;
	
	
</script>

<header>
	<div class="navbar bg-base-100">
		<div class="flex-1">
			<a href="/" class="btn btn-ghost normal-case text-xl">Transleet</a>
			<a href="/projects" class="btn btn-ghost normal-case text-sm">Projects</a>
		</div>
		<div class="flex-none gap-2">
			<div class="form-control">
				<input type="text" placeholder="Search" class="input input-bordered" />
			</div>
			<div class="dropdown dropdown-end">
				{#if islogin}
					<label tabindex="0" class="btn btn-ghost btn-circle avatar">
						<div class="w-10 rounded-full">
							<!-- <img src="https://api.lorem.space/image/face?hash=33791"> -->
						</div>
					</label>
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
						<li><a>Logout</a></li>
					</ul>
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
