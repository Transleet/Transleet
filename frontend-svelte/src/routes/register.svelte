<script lang="ts">
	import { goto } from '$app/navigation';
	import { userStore } from '$lib/stores';
	import { createForm } from 'svelte-forms-lib';
	import * as yup from 'yup';
	import axios from 'axios';
	import { AccountService, AuthorizationService } from '$lib/api';
	let username: string;
	let email: string;
	let password: string;
	let backend_base_url = import.meta.env.VITE_BACKEND_BASE_URL;
	let frontend_base_url = import.meta.env.VITE_FRONTEND_BASE_URL;
	const {
		form,
		errors,
		state,
		touched,
		isValid,
		isSubmitting,
		isValidating,
		handleReset,
		handleChange,
		handleSubmit
	} = createForm({
		initialValues: {
			username: '',
			email: '',
			password: ''
		},
		validationSchema: yup.object().shape({
			username: yup.string().required(),
			email: yup.string().email().required(),
			password: yup.string().required()
		}),
		onSubmit: async (values) => {
			let registerResponse = await AccountService.register(values);

			let loginResponse = await AuthorizationService.getToken({
				inputText: registerResponse.data.email,
				password: values.password
			});
			$userStore.token = loginResponse.data.token;
			$userStore.id = loginResponse.data.id;
			$userStore.isLogin = true;
			goto(frontend_base_url + '/register_confirm');
		}
	});
</script>

<div class="w-full max-w-xs">
	<form class:vaild={$isValid} on:submit={handleSubmit} class="form-control w-full max-w-xs">
		<label for="username" class="label"><span class="label-text">Username</span></label>
		<input
			id="username"
			name="username"
			class="input input-bordered w-full max-w-xs"
			on:keyup={handleChange}
			placeholder="Your username." />
		{#if $errors.username && $touched.username}
			<small>{$errors.username}</small>
		{/if}
		<label for="email" class="label"><span class="label-text">Email</span></label>
		<input
			id="email"
			name="email"
			class="input input-bordered w-full max-w-xs"
			on:keyup={handleChange}
			placeholder="Your email." />
		{#if $errors.email && $touched.email}
			<small>{$errors.email}</small>
		{/if}
		<label for="password" class="label"><span class="label-text">Password</span></label>
		<input
			id="password"
			name="password"
			class="input input-bordered w-full max-w-xs"
			on:keyup={handleChange}
			placeholder="Your password." />
		{#if $errors.password && $touched.password}
			<small>{$errors.password}</small>
		{/if}
		<button type="submit" disabled={!$isValid || !$state.isModified || $isValidating} class="btn">
			{#if $isSubmitting}
				loading...
			{:else}
				register
			{/if}
		</button>
	</form>
</div>
