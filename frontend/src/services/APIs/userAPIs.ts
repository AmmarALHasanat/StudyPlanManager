import api from "./api";
import {User} from "@/models/user";

class UserAPIs{
    async login(user: User) {
    return await api.post("/auth/login", user);
  }

  async register(user: User) {
    return await api.post("/auth/register", user);
  }
}

export default new UserAPIs();

