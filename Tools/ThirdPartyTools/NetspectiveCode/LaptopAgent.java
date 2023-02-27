package com.covidien;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.xml.bind.JAXBException;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.xpath.XPathExpressionException;

import org.xml.sax.SAXException;

import com.covidien.laptopagent.log.LoggingService;
import com.covidien.laptopagent.message.MessageService;
import com.covidien.laptopagent.service.AgentAPI;
import com.covidien.laptopagent.service.LaptopAgentFactory;
import com.covidien.laptopagent.xml.XMLMessage;
import com.covidien.laptopagent.xml.XMLMessageProcesser;

/**
 * Main Service class.
 * @author prakash
 */
public final class LaptopAgent {
	
	/**
	 * SOCKET_PORT_NUMEBR.
	 */
	private static final int SOCKET_PORT_NUMEBR = 9999;
	/**
	 * Server Socket.
	 */
	private ServerSocket server;
	/**
	 * Client Socket.
	 */
	private Socket clientSocket;
	/**
	 * Object InputStream.
	 */
	private ObjectInputStream in;
	/**
	 * ObjectOutputStream.
	 */
	private ObjectOutputStream out;
	/**
	 * AgentAPI class.
	 */
	private AgentAPI api;
	
	/**
	 * Service stop method.
	 * @throws IOException 
	 */
	public void stop() throws IOException {
		// TODO Auto-generated method stub
		//api = LaptopAgentFactory.getAgentAPI("1.0");
		
		if(in!=null) {
			in.close();
		}
		if(out!=null) {
			out.close();
		}
		if(clientSocket!=null) {
			clientSocket.close();
		}
		if(server!=null) {
			server.close();
		}

		
		// Use to api to relieve resources.
	}

	/**
	 * Service start method. 
	 * @throws IOException 
	 */
	public void start() throws IOException {
		
		api = LaptopAgentFactory.getAgentAPI("1.0");
		// Use api object to perform the respective actions.
		
		
		server = LaptopAgentSocket.createServerSocket(SOCKET_PORT_NUMEBR);
		
		do {
			clientSocket = LaptopAgentSocket.createConnection(server);
		
			in = LaptopAgentSocket.getInputStream(clientSocket);
			out = LaptopAgentSocket.getOutputStream(clientSocket);
			
			try {
					//sendMessage("Connection successful");
					String input = in.readObject().toString();
					ByteArrayInputStream byteInput = new ByteArrayInputStream(input.getBytes());
					XMLMessageProcesser processor = new XMLMessageProcesser();
					XMLMessage message = processor.parseMessage(byteInput);
					XMLMessage response = processMessage(message);
					String resMes = XMLMessageProcesser.marshallToString(response);
					out.writeObject(resMes);
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				try {
					out.writeObject("IOException");
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
			} catch (ClassNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				try {
					out.writeObject("ClassNotFoundException");
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
			} catch (Exception e) {
				try {
					out.writeObject(e.getMessage());
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
			}
			
		} while (true);	
	}
	/**
	 * Proceess the request based on the message type. 
	 * @param message .
	 * @return XMLMessage.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws NoSuchAlgorithmException .
	 * @throws JAXBException .
	 * @throws InvalidKeyException .
	 * @throws XPathExpressionException .
	 * @throws IllegalBlockSizeException .
	 * @throws BadPaddingException .
	 * @throws NoSuchPaddingException .
	 * @throws InvalidKeySpecException .
	 * @throws SAXException .
	 * @throws IOException .
	 */
	private XMLMessage processMessage(final XMLMessage message) throws ParserConfigurationException, TransformerException, 
		NoSuchAlgorithmException, JAXBException, InvalidKeyException, XPathExpressionException, IllegalBlockSizeException, 
		BadPaddingException, NoSuchPaddingException, InvalidKeySpecException, SAXException, IOException {
		// TODO Auto-generated method stub
		XMLMessage response = null;
		LoggingService logservice = new LoggingService();
		logservice.loggingMessage(message);
		
		if(message.getHeaderRequest()!=null) {
			if("createsession".equals(message.getHeaderRequest().getType())) {
				response = api.createSession(message);
			} else if("opensession".equals(message.getHeaderRequest().getType())) {
				response = api.openSession(message);
			} else if("closesession".equals(message.getHeaderRequest().getType())) {
				response = api.closeSession(message);
			} else if("login".equals(message.getHeaderRequest().getType())) {
				response = api.loginSession(message);
			} else if("logoff".equals(message.getHeaderRequest().getType())) {
				response = api.logoffSession(message);
			} else if("createdevice".equals(message.getHeaderRequest().getType())) {
				response = api.createDevice(message);
			} else if("deletedevice".equals(message.getHeaderRequest().getType())) {
				response = api.deleteDevice(message);
			} else if("statdevice".equals(message.getHeaderRequest().getType())) {
				response = api.statDevice(message);
			} else if("getheaders".equals(message.getHeaderRequest().getType())) {
				response = api.getHeaders(message);
			} else if("getnotification".equals(message.getHeaderRequest().getType())) {
				response = api.getNotification(message);
			} else if("postnotification".equals(message.getHeaderRequest().getType())) {
				response = api.postNotification(message);
			} else if("deletenotification".equals(message.getHeaderRequest().getType())) {
				response = api.deleteNotification(message);
			} else if("updatenotification".equals(message.getHeaderRequest().getType())) {
				response = api.updateNotification(message);
			} else if("undeletenotification".equals(message.getHeaderRequest().getType())) {
				response = api.undeleteNotification(message);
			} else if("expunge".equals(message.getHeaderRequest().getType())) {
				response = api.expungeNotification(message);
			}
		}
		
		// Just for testing. Will be removed later.
		if(message.getHeaderResponse()!=null) {
			MessageService service = new MessageService();
			if(message.getHeaderResponse().getType().equals("notifications")) {
				response = api.deviceUpdate(message);
			}
			String deviceSerialNumber = getDeviceSerialNumber(message);
			if(deviceSerialNumber!=null) { 
				service.storeMessage(getDeviceSerialNumber(message), message);
			} else {
				service.storeMessage(message);
			}
			response = message;
		}
		
		return response;
	}
	
	/**
	 * Get device serial number from xml file.
	 * @param message Message from VTS.
	 * @return string Device Serial Number.
	 */
	public String getDeviceSerialNumber(final XMLMessage message) {
		String deviceSerialNo = null;
		if(message.getHeaderResponse().getParams()==null) {
			return deviceSerialNo;
		}
		if (message.getHeaderResponse().getParams().getMailBox()!=null) {
			deviceSerialNo = message.getHeaderResponse().getParams().getMailBox();
		}
		return deviceSerialNo;
	}
}
