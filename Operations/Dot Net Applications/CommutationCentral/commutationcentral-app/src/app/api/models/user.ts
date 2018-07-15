import { UserRole } from "./user-role";

export class User {
    id: string = "";
    userName: string;
    email: string;
    userRoles: UserRole[];
}
