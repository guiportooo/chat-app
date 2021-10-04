import React from 'react'

import Message from './Message'

const ChatWindow = (props) => {
  const chat = props.chat.map((m) => (
    <Message
      key={Date.now() * Math.random()}
      user={m.userName}
      message={m.text}
      time={m.timestamp}
    />
  ))

  return <div>{chat}</div>
}

export default ChatWindow
