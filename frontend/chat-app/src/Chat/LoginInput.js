import React, { useState } from 'react'

const LoginInput = (props) => {
  const [user, setUser] = useState('')
  const [password, setPassword] = useState('')

  const onLogin = (e) => {
    e.preventDefault()

    const isUserProvided = user && user !== ''
    const isPasswordProvided = password && password !== ''

    if (isUserProvided && isPasswordProvided) {
      props.logIn(user, password)
    } else {
      alert('Please insert an user and a password.')
    }
  }

  const onRegister = (e) => {
    e.preventDefault()

    const isUserProvided = user && user !== ''
    const isPasswordProvided = password && password !== ''

    if (isUserProvided && isPasswordProvided) {
      props.register(user, password)
    } else {
      alert('Please insert an user and a password.')
    }
  }

  const onUserUpdate = (e) => setUser(e.target.value)

  const onPasswordUpdate = (e) => setPassword(e.target.value)

  return (
    <form>
      <label htmlFor='user'>User:</label>
      <br />
      <input
        type='text'
        id='user'
        name='user'
        value={user}
        onChange={onUserUpdate}
      />
      <br />
      <label htmlFor='password'>Password:</label>
      <br />
      <input
        type='password'
        id='password'
        name='password'
        value={password}
        onChange={onPasswordUpdate}
      />
      <br />
      <br />
      <button onClick={onLogin}>LogIn</button>
      <button onClick={onRegister}>Register</button>
    </form>
  )
}

export default LoginInput
