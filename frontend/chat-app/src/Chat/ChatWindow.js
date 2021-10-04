import React from 'react'

import Message from './Message'

const ChatWindow = (props) => {
  const chat = props.chat.map((m) => (
    <Message
      key={Date.now() * Math.random()}
      user={m.userName}
      message={m.text}
    />
  ))

  return <div>{chat}</div>
}

export default ChatWindow
