import React, { useState } from 'react'

const ChatInput = (props) => {
  const [message, setMessage] = useState('')

  const onSubmit = (e) => {
    e.preventDefault()

    const isMessageProvided = message && message !== ''

    if (isMessageProvided) {
      props.sendMessage(message)
    } else {
      alert('Please insert a message.')
    }
  }

  const onMessageUpdate = (e) => setMessage(e.target.value)

  return (
    <form onSubmit={onSubmit}>
      <label htmlFor='message'>Message:</label>
      <br />
      <input
        type='text'
        id='message'
        name='message'
        value={message}
        onChange={onMessageUpdate}
      />
      <br />
      <br />
      <button>Submit</button>
    </form>
  )
}

export default ChatInput
