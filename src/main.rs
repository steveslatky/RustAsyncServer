extern crate bytes;
extern crate futures;
extern crate tokio_io;
extern crate tokio_proto;
extern crate tokio_service;

use std::io;
use std::str;
use bytes::BytesMut;
use tokio_io::codec::{Encoder, Decoder};

pub struct LineCodec;

impl Decoder for LineCodec {
    type Item = String;
    type Error = io::Error;

    // ...
}

impl Encoder for LineCodec {
    type Item = String;
    type Error = io::Error;

    // ...
}

fn main() {
    println!("Hello, world!");
}

