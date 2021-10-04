import React, { useState, useEffect, useRef } from 'react'
import { HubConnectionBuilder } from '@microsoft/signalr'

import LoginInput from './LoginInput'
import ChatInput from './ChatInput'
import ChatWindow from './ChatWindow'

const Chat = () => {
  const [token, setToken] = useState('')
  const [chat, setChat] = useState([])
  const latestChat = useRef(null)

  latestChat.current = chat

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/hubs/chat')
      .withAutomaticReconnect()
      .build()

    connection
      .start()
      .then((result) => {
        console.log('Connected!')

        connection.on('ReceiveMessage', (message) => {
          const updatedChat = [...latestChat.current]
          updatedChat.push(message)

          setChat(updatedChat)
        })
      })
      .catch((e) => console.log('Connection failed: ', e))
  }, [])

  const logIn = async (userName, password) => {
    const authenticateUser = {
      userName: userName,
      password: password,
    }

    try {
      const response = await fetch('https://localhost:5001/authenticate', {
        method: 'POST',
        body: JSON.stringify(authenticateUser),
        headers: {
          'Content-Type': 'application/json',
        },
      })
      var authenticatedUser = await response.json()
      setToken(authenticatedUser.token)
    } catch (e) {
      console.log('Login failed.', e)
    }
  }

  const sendMessage = async (message) => {
    const chatMessage = {
      text: message,
    }

    try {
      const response = await fetch(
        'https://localhost:5001/rooms/general/messages',
        {
          method: 'POST',
          body: JSON.stringify(chatMessage),
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
        }
      )
      console.log('sendMessageResponse', await response.json())
    } catch (e) {
      console.log('Sending message failed.', e)
    }
  }

  return (
    <div>
      {token.length > 0 ? (
        <div>
          <ChatInput sendMessage={sendMessage} />
          <hr />
          <ChatWindow chat={chat} />
        </div>
      ) : (
        <LoginInput logIn={logIn} />
      )}
    </div>
  )
}

export default Chat
