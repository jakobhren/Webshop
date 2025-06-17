export interface Login {
    headerValue: string;
    customer: {
        id: number;
        name: string;
        email: string;
        password: string;
        creation: Date;
    }
}
