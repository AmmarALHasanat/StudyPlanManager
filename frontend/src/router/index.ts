import { createRouter, createWebHistory } from "vue-router";
import LoginView from "../views/LoginView.vue";
import MainLayout from "../layouts/MainLayout.vue";
import PlansView from "../views/PlansView.vue";
import WeeksView from "../views/WeeksView.vue";
import TasksView from "../views/TasksView.vue";
import DashboardView from "../views/DashboardView.vue"
const routes = [
  { path: "/", redirect: "/login" },

  { path: "/login", component: LoginView },

  {
    path: "/",
    component: MainLayout,
    children: [
      { path: "/plans", component: PlansView },
      { path: "/weeks/:id", component: WeeksView },
      { path: "/tasks/:id", component: TasksView },
      { path: "/dashboard", component: DashboardView },
    ],
  },
];


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
