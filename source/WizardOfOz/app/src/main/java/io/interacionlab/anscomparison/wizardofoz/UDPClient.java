package io.interacionlab.anscomparison.wizardofoz;

import android.os.AsyncTask;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

public class UDPClient extends AsyncTask<String, Void, Void> {

    private InetAddress ip;
    private int port;

    public UDPClient(InetAddress ip, int port){
        this.ip = ip;
        this.port = port;
    }

    @Override
    protected Void doInBackground(String... params) {
        DatagramSocket ds = null;
        try {
            ds = new DatagramSocket();
            DatagramPacket dp;
            dp = new DatagramPacket(params[0].getBytes(), params[0].length(), ip, port);
            ds.setBroadcast(true);
            ds.send(dp);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            if (ds != null) {
                ds.close();
            }
        }
        return null;
    }

    protected void onPostExecute(Void result) {
        super.onPostExecute(result);
    }
}
