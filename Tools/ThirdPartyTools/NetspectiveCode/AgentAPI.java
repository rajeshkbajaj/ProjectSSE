package com.covidien.laptopagent.service;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
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

import com.covidien.laptopagent.xml.XMLMessage;

/**
 * Agent API interface.
 * @author prakash
 *
 */
public interface AgentAPI {
	
	/**
	 * Create a session.
	 * @param message session request object
	 * @return created session response object
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws IOException .
	 */
	XMLMessage createSession(final XMLMessage message) throws ParserConfigurationException, TransformerException, IOException;
	
	/**
	 * Open a session.
	 * @param message request message to open a session.
	 * @return opened session response.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws IOException .
	 */
	XMLMessage openSession(final XMLMessage message) throws ParserConfigurationException, TransformerException, IOException;
	
	/**
	 * Close a session.
	 * @param message request to a close a sessoin.
	 * @return closed session response. 
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws FileNotFoundException .
	 */
	XMLMessage closeSession(final XMLMessage message) throws ParserConfigurationException, TransformerException, FileNotFoundException ;
	
	/**
	 * Login using the username and password to the session.
	 * @param message request to login into the session.
	 * @return response of the login message.
	 * @throws NoSuchAlgorithmException .
	 * @throws UnsupportedEncodingException .
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws FileNotFoundException .
	 */
	XMLMessage loginSession(final XMLMessage message) throws NoSuchAlgorithmException, UnsupportedEncodingException, 
		ParserConfigurationException, TransformerException, FileNotFoundException;
	
	/**
	 * Logoff the logged in session.
	 * @param message request to loggoff the session.
	 * @return response to logof session.
	 * @throws NoSuchAlgorithmException .
	 * @throws UnsupportedEncodingException .
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws FileNotFoundException .
	 */
	XMLMessage logoffSession(final XMLMessage message) throws NoSuchAlgorithmException, UnsupportedEncodingException, 
		ParserConfigurationException, TransformerException, FileNotFoundException;
	
	/**
	 * Create a new device.
	 * @param message request to create new device.
	 * @return response to the create device.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws IOException .
	 */
	XMLMessage createDevice(final XMLMessage message) throws ParserConfigurationException, TransformerException, IOException;
	
	/**
	 * Delete the device.
	 * @param message request to delete the device.
	 * @return response to delete the device.
	 * @throws ParserConfigurationException . 
	 * @throws TransformerException .
	 */
	XMLMessage deleteDevice(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	
	/**
	 * Stat the device information.
	 * @param message request to get the stat of the device.
	 * @return response of the stat device.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 */
	XMLMessage statDevice(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	
	/**
	 * Get headers request to check for any new messages.
	 * @param message headers request message.
	 * @return response for header request. 
	 * @throws FileNotFoundException .
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws JAXBException .
	 */
	XMLMessage getHeaders(final XMLMessage message) throws FileNotFoundException, ParserConfigurationException, TransformerException, JAXBException;
	
	/**
	 * Get a information about the particular response.
	 * @param message get notification request.
	 * @return response for the given notification request.
	 * @throws FileNotFoundException .
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 * @throws JAXBException .
	 */
	XMLMessage getNotification(final XMLMessage message) throws FileNotFoundException, ParserConfigurationException, TransformerException, JAXBException;
	
	/**
	 * Post the log information notification.
	 * @param message request message to post log.
	 * @return response of the log post status.
	 * @throws InvalidKeyException .
	 * @throws XPathExpressionException .
	 * @throws IllegalBlockSizeException .
	 * @throws BadPaddingException .
	 * @throws NoSuchAlgorithmException .
	 * @throws NoSuchPaddingException .
	 * @throws InvalidKeySpecException .
	 * @throws ParserConfigurationException .
	 * @throws SAXException .
	 * @throws IOException .
	 * @throws TransformerException .
	 */
	XMLMessage postNotification(final XMLMessage message) throws InvalidKeyException, XPathExpressionException, IllegalBlockSizeException, 
			BadPaddingException, NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeySpecException, ParserConfigurationException, SAXException, IOException, TransformerException;
	
	/**
	 * deleteNotification.
	 * @param message .
	 * @return Message .
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 */
	XMLMessage deleteNotification(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	/**
	 * updateNotification.
	 * @param message .
	 * @return Message .
 	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 */
	XMLMessage updateNotification(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	/**
	 * undeleteNotification.
	 * @param message .
	 * @return Message.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 */
	XMLMessage undeleteNotification(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	/**
	 * expungeNotification.
	 * @param message .
	 * @return Message.
	 * @throws ParserConfigurationException .
	 * @throws TransformerException .
	 */
	XMLMessage expungeNotification(final XMLMessage message) throws ParserConfigurationException, TransformerException;
	
	/**
	 * Update device with software or document.
	 * @param message with download url.
	 * @return updated message with local filepath.
	 */
	XMLMessage deviceUpdate(final XMLMessage message);
	
	/**
	 * Get implemetation version.
	 * @return version.
	 */
	String getVersion();
}
