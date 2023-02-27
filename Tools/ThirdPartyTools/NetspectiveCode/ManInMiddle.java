package com.covidien.laptopagent.mim;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.Properties;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.xml.bind.JAXBException;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.xpath.XPathExpressionException;

import org.xml.sax.SAXException;

import com.covidien.laptopagent.LoadProperties;
import com.covidien.laptopagent.log.LoggingParser;
import com.covidien.laptopagent.log.xml.LogMessage;
import com.covidien.laptopagent.log.xml.XMLLogMessage;
import com.covidien.laptopagent.xml.XMLMessage;
import com.covidien.laptopagent.xml.XMLMessageProcesser;

/**
 * Man in Middle Module.
 * @author Viswa
 *
 */
public final class ManInMiddle {

	/**
	 * Port number.
	 */
	private static final int SOCKET_PORT = 9999;
	/**
	 * Private Constructor.
	 */
	private ManInMiddle(){

	}
	/**
	 * Message reproducing time wait.
	 */
	private static final int PLAYWAIT = 3000;

	/**
	 * Maintaining user properties defined the external file.
	 */
	private static Properties properties = new Properties();
	static {
		properties = LoadProperties.loadProperties("resource" + File.separator +"conf.properties");
	}

	/**
	 * Message reproducer.
	 * @throws TransformerException 
	 * @throws ParserConfigurationException 
	 * @throws NoSuchAlgorithmException 
	 * @throws SAXException 
	 * @throws JAXBException 
	 * @throws InvalidKeySpecException 
	 * @throws NoSuchPaddingException 
	 * @throws BadPaddingException 
	 * @throws IllegalBlockSizeException 
	 * @throws XPathExpressionException 
	 * @throws InvalidKeyException 
	 * @throws InterruptedException .
	 * @throws IOException .
	 */
	public static void messageSender() throws IOException, InterruptedException, ParserConfigurationException, TransformerException, NoSuchAlgorithmException, InvalidKeyException, XPathExpressionException, IllegalBlockSizeException, BadPaddingException, NoSuchPaddingException, InvalidKeySpecException, JAXBException, SAXException {
		LogMessage loggingMessage=LoggingParser.parseMessage(new File(properties.getProperty("currentPath") + 
				properties.getProperty("AGENT.API.XML")));
		
		ArrayList<XMLLogMessage> arrayLogMessage=loggingMessage.getMessages();
		Iterator<XMLLogMessage> iter = arrayLogMessage.iterator();
		while (iter.hasNext()) {
			XMLLogMessage xmlLog = iter.next();
			String cdata=(xmlLog.getCDATA().replaceAll("<!\\[CDATA\\[", "")).replaceAll("]]>", "");
			ByteArrayInputStream byteStream = new ByteArrayInputStream(cdata.toString().getBytes());
			XMLMessage toPass=LoggingParser.parseMessage(byteStream);
			sendToAPI(toPass);
			new File(properties.getProperty("AGENT.API.TEMP.MSG")).delete();
			Thread.sleep(PLAYWAIT);
		}
	}
	
	/**
	 * Call the API through socket.
	 * @param message to be sent
	 */
	private static void sendToAPI(final XMLMessage message) {
		Socket client = null;
		ObjectInputStream in = null;
		ObjectOutputStream out = null;
		try {
			client = new Socket("localhost", SOCKET_PORT);
			String input = XMLMessageProcesser.marshallToString(message);
			out = new ObjectOutputStream(client.getOutputStream());
			out.writeObject(input);
			out.flush();
			in = new ObjectInputStream(client.getInputStream());
			String response = in.readObject().toString();
			System.out.println(response);
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
//			e.printStackTrace();
			return;
		} catch (IOException e) {
			// TODO Auto-generated catch block
//			e.printStackTrace();
			return;
		} catch (ClassNotFoundException e) {
			// TODO Auto-generated catch block
//			e.printStackTrace();
			return;
		} finally {
			try {
				if(client != null) {
					client.close();
				}
				if(in != null) {
					in.close();
				}
				if(out != null) {
					out.close();
				}
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	/**
	 * Main Function.
	 * @param arg .
	 * @throws TransformerException 
	 * @throws ParserConfigurationException 
	 * @throws NoSuchAlgorithmException 
	 * @throws SAXException 
	 * @throws JAXBException 
	 * @throws InvalidKeySpecException 
	 * @throws NoSuchPaddingException 
	 * @throws BadPaddingException 
	 * @throws IllegalBlockSizeException 
	 * @throws XPathExpressionException 
	 * @throws InvalidKeyException 
	 * @throws InterruptedException .
	 * @throws IOException .
	 */
	public static void main(final String[] arg) throws IOException, InterruptedException, ParserConfigurationException, 
		TransformerException, NoSuchAlgorithmException, InvalidKeyException, XPathExpressionException, IllegalBlockSizeException, 
		BadPaddingException, NoSuchPaddingException, InvalidKeySpecException, JAXBException, SAXException{
		messageSender();
	}
}
