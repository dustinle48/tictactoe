import { FormEvent } from "react";

type UserFormProps = {
  label: "Log In" | "Sign Up";
  onSubmit: (event: FormEvent) => void;
};

const UserForm = (props: UserFormProps) => {
  const { label, onSubmit } = props;

  return (
    <form onSubmit={onSubmit} method="post">
      <input name="name" placeholder="Username" />
      <input name="password" placeholder="Password" type="password" />
      <button type="submit">{label}</button>
    </form>
  );
};

export default UserForm;
