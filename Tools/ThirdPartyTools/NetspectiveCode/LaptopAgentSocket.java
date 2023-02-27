package com.covidien;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * Laptop Agent Socket Service.
 * @author Prakash
 *
 */
public final class LaptopAgentSocket {
	/**
	 * Private constructor.
	 */
	private LaptopAgentSocket(){
		
	}
	/**
	 * Create SocketServerSocket.
	 * @param portNo .
	 * @return ServerSocket.
	 */
	public static ServerSocket createServerSocket(final int portNo) {
		ServerSocket server = null;
		try {
			server = new ServerSocket(portNo);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return server;
	}
	/**
	 * Create Connection.
	 * @param server .
	 * @return Socket.
	 */
	public static Socket createConnection(final ServerSocket server) {
		Socket connectSocket = null;
		try {
			connectSocket = server.accept();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return connectSocket;
	}
	/**
	 * Get Input Stream.
	 * @param socket .
	 * @return ObjectInputStream.
	 */
	public static ObjectInputStream getInputStream(final Socket socket) {
		ObjectInputStream in = null;
		try {
			in = new ObjectInputStream(socket.getInputStream());
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return in;
	}
	/**
	 * Get Output Stream.
	 * @param socket .
	 * @return ObjectOutputStream.
	 */
	public static ObjectOutputStream getOutputStream(final Socket socket) {
		ObjectOutputStream out = null;
		try {
			out = new ObjectOutputStream(socket.getOutputStream());
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return out;
	}
	
	

}
