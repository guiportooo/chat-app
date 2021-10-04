import React from 'react'
import moment from 'moment'

const Message = (props) => (
  <div style={{ background: '#eee', borderRadius: '5px', padding: '0 10px' }}>
    <p>
      <strong>{props.user}</strong> says{' '}
      {moment(props.time).format('h:mm:ss a')}:
    </p>
    <p>{props.message}</p>
  </div>
)

export default Message
