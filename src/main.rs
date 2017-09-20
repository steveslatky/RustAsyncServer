extern crate mio;

use mio::*;
use mio::tcp::{TcpListener, TcpStream};
use std::time::Duration;

const SERVER: Token = Token(0);
const CLIENT: Token = Token(1);

fn start_server() {

    let listener = TcpListener::bind(&"127.0.0.1:12345".parse().unwrap()).unwrap();
    
    // Poll is a way to see when events are called. 
    let poll = Poll::new().unwrap();
    let mut events = Events::with_capacity(128);

    // Register the socket with `Poll`
    poll.register(&listener, Token(0), Ready::writable(),
    PollOpt::edge()).unwrap();

    poll.poll(&mut events, Some(Duration::from_millis(100))).unwrap();

    // There may be a socket ready to be accepted
    println!("Waiting for connection...");
}


fn main(){
   start_server();  

}

