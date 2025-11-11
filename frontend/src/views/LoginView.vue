<template>
  <div class="d-flex justify-content-center align-items-center vh-100 bg-light">
    <div class="card shadow p-4" style="width: 350px">
      <h4 class="text-center mb-3 fw-bold">Login</h4>
      <form @submit.prevent="login">
        <div class="mb-3">
          <input v-model="username" class="form-control" placeholder="Username" required />
        </div>
        <div class="mb-3">
          <input
            v-model="password"
            type="password"
            class="form-control"
            placeholder="Password"
            required
          />
        </div>
        <button class="btn btn-primary w-100">Login</button>
        <p v-if="error" class="text-danger mt-3 text-center">{{ error }}</p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { User } from "@/models/user";
import UserAPIs from "@/services/APIs/userAPIs";
const router = useRouter();
const username = ref("");
const password = ref("");
const error = ref("");

async function login() {
  const user = new User();
  user.username= username.value
  user.password= password.value
  try {
    const res = await UserAPIs.login(user);
    localStorage.setItem("token", res.data.token);
    router.push("/plans");
  } catch (err) {
    error.value = "Invalid username or password";
  }
}
</script>
