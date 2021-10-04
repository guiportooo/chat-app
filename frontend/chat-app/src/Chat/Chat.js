import React, { useState, useEffect, useRef } from 'react'
import { HubConnectionBuilder } from '@microsoft/signalr'

import LoginInput from './LoginInput'
import ChatInput from './ChatInput'
import ChatWindow from './ChatWindow'

const Chat = () => {
  const [token, setToken] = useState(null)
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
          updatedChat.unshift(message)

          setChat(updatedChat)
        })
      })
      .catch((e) => console.log('Connection failed: ', e))
  }, [])

  const register = async (userName, password) => {
    const registerUser = {
      userName: userName,
      password: password,
    }

    try {
      const response = await fetch('https://localhost:5001/register', {
        method: 'POST',
        body: JSON.stringify(registerUser),
        headers: {
          'Content-Type': 'application/json',
        },
      })

      if (response.ok) {
        alert(`${userName} registered.`)
      } else {
        alert('Registering failed.')
      }
    } catch (e) {
      console.log('Registering failed.', e)
    }
  }

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

      if (response.ok) {
        var authenticatedUser = await response.json()
        setToken(authenticatedUser.token)
        alert(`${userName} logged in.`)
      } else {
        alert('Login failed.')
      }
    } catch (e) {
      console.log('Login failed.', e)
    }
  }

  useEffect(() => {
    if (token) {
      const fetchData = async () => {
        try {
          const response = await fetch(
            'https://localhost:5001/rooms/general/messages',
            {
              method: 'GET',
              headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
              },
            }
          )

          var content = await response.json()
          var messages = content.messages

          const updatedChat = [...latestChat.current]
          messages.map((message) => updatedChat.push(message))

          setChat(updatedChat)
        } catch (e) {
          console.log('Retrieving messages failed.', e)
        }
      }

      fetchData().catch(console.error)
    }
  }, [token])

  const sendMessage = async (message) => {
    const chatMessage = {
      text: message,
    }

    try {
      await fetch('https://localhost:5001/rooms/general/messages', {
        method: 'POST',
        body: JSON.stringify(chatMessage),
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      })
    } catch (e) {
      console.log('Sending message failed.', e)
    }
  }

  return (
    <div>
      {token ? (
        <div>
          <ChatInput sendMessage={sendMessage} />
          <hr />
          <ChatWindow chat={chat} />
        </div>
      ) : (
        <LoginInput logIn={logIn} register={register} />
      )}
    </div>
  )
}

export default Chat
